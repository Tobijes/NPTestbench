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
            const response = await fetch('http://localhost:5000/api/Configuration/' + currentConfiguration.id + '/UpdateParameter/', {
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

    const addParameter = async () => {
        console.log("paramter being added")
        try {
            const response = await fetch('http://localhost:5000/api/Configuration/' + currentConfiguration.id + '/AddParameter/', {
                method: 'POST', // Use 'POST' or 'PUT', depending on your API requirements
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({
                    Name: "",
                    Value: "",
                }),
            });

            if (!response.ok) {
                throw new Error('Failed to update the active configuration on the server');
            }
            var addedParameterId = await response.json()
            setCurrentConfiguration((prevState) => {
                console.log("prior state.:" + JSON.stringify(prevState))
                return {
                    ...prevState,
                    parameters: [
                        ...prevState.parameters,
                        { id: addedParameterId, name: "", value: "" }
                    ],
                };
            });

        } catch (error) {
            console.error('Error updating the active configuration:', error);
        }

    };


    const deleteParameter = async (parameterId) => {
        var newParameters = currentConfiguration.parameters.filter((param) => param.id !== parameterId);
        setCurrentConfiguration((prevState) => ({
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
            console.log("parameter has been beenDeleted " + parameterId)
            if (!response.ok) {
                throw new Error('Failed to delete parameter');
            }

        } catch (error) {
            console.error('Failed to delete parameter:', error);
        }

    };

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

    const getConfigById = async (configId) => {
        try {
            const response = await fetch('http://localhost:5000/api/Configuration/GetConfigById/' + configId, {
                method: 'GET', // Use 'POST' or 'PUT', depending on your API requirements
                headers: {
                    'Content-Type': 'application/json',
                }, // Send the active configuration as the request body
            });
            if (!response.ok) {
                throw new Error('Failed to fetch config by id');
            }
            const data = await response.json()
            setCurrentConfiguration(data);


        } catch (error) {
            console.error('Failed to fetch config by id:', error);
        }

    };


    const createConfiguration = async (configName) => {
        try {
            const response = await fetch('http://localhost:5000/api/Configuration/CreateConfiguration/', {
                method: 'POST', // Use 'POST' or 'PUT', depending on your API requirements
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({
                    Name: configName
                })

            });
            if (!response.ok) {
                throw new Error('Failed to create config');
            }
            const data = await response.json()
            setCurrentConfiguration(data);
            setConfigurations([...configs, data])


        } catch (error) {
            console.error('Failed to create config:', error);
        }

    };
    return (
        <ConfigurationContext.Provider value={
            {
                activeConfiguration,
                setActiveConfiguration,
                configs,
                setConfigurations,
                updateParameter,
                addParameter,
                deleteParameter,
                currentConfiguration,
                setCurrentConfiguration,
                getConfigById,
                createConfiguration
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