import { useState, useEffect, createContext, useContext } from 'react';
import PropTypes from 'prop-types';



const ConfigurationProvider = (props) => {
    const [activeConfiguration, setActiveConfiguration] = useState(null);

    const [configs, setConfigurations] = useState([])

    const [currentConfiguration, setCurrentConfiguration] = useState(null)



    useEffect(() => {
        // This function will update the server with the new active configuration
        const updateServerActiveConfig = async () => {
            if (activeConfiguration) { // Ensure there is an active configuration to update
                try {

                    const response = await fetch('http://localhost:5000/api/Configuration/SetActiveConfiguration/' + activeConfiguration.id, {
                        method: 'POST', // Use 'POST' or 'PUT', depending on your API requirements
                        headers: {
                            'Content-Type': 'application/json',
                        },
                        //body: JSON.stringify(state.id), // Send the active configuration as the request body
                    });
                    console.log("active config has been set")
                    if (!response.ok) {
                        throw new Error('Failed to update the active configuration on the server');
                    }

                } catch (error) {
                    console.error('Error updating the active configuration:', error);
                }
            }
        };

        if (activeConfiguration !== null) { // Check if the state is not null to avoid running on initial load
            updateServerActiveConfig();
        }
    }, [activeConfiguration]); // Run this effect when `state` changes



    const updateParameter = async (parameter) => {
        setCurrentConfiguration((prevState) => {
            // Check if prevState and parameters exist to avoid null reference errors
            if (!prevState || !prevState.parameters) return prevState;
            const newParameters = prevState.parameters.map((param) => {
                // If the parameter ID matches, return the updated parameter, else return the original
                console.log()
                return param.id === parameter.id ? parameter : param;
            });
            console.log("parameters has been added " + JSON.stringify(newParameters))
            return {
                ...prevState,
                parameters: newParameters,
            };
        });
       

        console.log("this is paramter to update: " + JSON.stringify(parameter))
        try {
            const response = await fetch('http://localhost:5000/api/Configuration/' + activeConfiguration.id + '/UpdateParameter/', {
                method: 'POST', // Use 'POST' or 'PUT', depending on your API requirements
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({
                    Id: parameter.id,
                    Name: parameter.name,
                    Value: parameter.value
                }), // Send the active configuration as the request body
            });
            if (!response.ok) {
                throw new Error('Failed to update the active configuration on the server');
            }

        } catch (error) {
            console.error('Error updating the active configuration:', error);
        }

    };


    const deleteParameter = async (parameterId) => {
        var newParameters = activeConfiguration.parameters.filter((param) => param.id !== parameterId);
        setActiveConfiguration((prevState) => ({
            ...prevState,
            parameters: newParameters,
        }));
        console.log("this is parm" + parameterId)
        try {
            const response = await fetch('http://localhost:5000/api/Configuration/' + parameterId + '/DeleteParameter/', {
                method: 'POST', // Use 'POST' or 'PUT', depending on your API requirements
                headers: {
                    'Content-Type': 'application/json',
                }, // Send the active configuration as the request body
            });
            console.log("parameters has been added " + newParameters)
            if (!response.ok) {
                throw new Error('Failed to delete parameter');
            }

        } catch (error) {
            console.error('Failed to delete parameter:', error);
        }

    };

    useEffect(() => {
        console.log("configs added")
    }, [configs]);

    useEffect(() => {
        const fetchActiveConfig = async () => {
            try {
                const response = await fetch('http://localhost:5000/api/Configuration');
                const data = await response.json();
                setActiveConfiguration(data);
                setCurrentConfiguration(data)
            } catch (error) {
                console.error('Error fetching data: ', error);
            }
        };

        fetchActiveConfig();
    }, []);

    useEffect(() => {
        const fetchConfigs = async () => {
            try {
                const response = await fetch('http://localhost:5000/api/Configuration/list');
                const data = await response.json();
                setConfigurations(data);
            } catch (error) {
                console.error('Error fetching data: ', error);
            }
        };

        fetchConfigs();
    }, []);

    return (
        <ConfigurationContext.Provider value={
            {
                activeConfiguration,
                setActiveConfiguration,
                configs,
                setConfigurations,
                updateParameter,
                deleteParameter,
                currentConfiguration,
                setCurrentConfiguration
            }
        }>
            {props.children}
        </ConfigurationContext.Provider>
    );
};


ConfigurationProvider.propTypes = {
    children: PropTypes.node.isRequired,
};

const ConfigurationContext = createContext();

export const useConfigurationContext = () => useContext(ConfigurationContext);

export default ConfigurationProvider;