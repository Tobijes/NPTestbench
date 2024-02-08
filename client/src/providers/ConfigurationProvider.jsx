import { useState, useEffect, createContext, useContext } from 'react';
import PropTypes from 'prop-types';



const ConfigurationProvider = (props) => {
    const [state, setState] = useState(null);
    const [configs, loadConfigs] = useState([])


   /* useEffect(() => {
        // This function will update the server with the new active configuration
        const updateServerActiveConfig = async () => {
            if (state) { // Ensure there is an active configuration to update
                try {
                    const response = await fetch('http://localhost:5000/api/Configuration/active', {
                        method: 'POST', // Use 'POST' or 'PUT', depending on your API requirements
                        headers: {
                            'Content-Type': 'application/json',
                        },
                        body: JSON.stringify(state), // Send the active configuration as the request body
                    });

                    if (!response.ok) {
                        throw new Error('Failed to update the active configuration on the server');
                    }

                    // Optionally, process the response
                    const data = await response.json();
                    console.log('Server updated successfully:', data);
                } catch (error) {
                    console.error('Error updating the active configuration:', error);
                }
            }
        };

        if (state !== null) { // Check if the state is not null to avoid running on initial load
            updateServerActiveConfig();
        }
    }, [state]); // Run this effect when `state` changes*/



    useEffect(() => {
        const fetchActiveConfig = async () => {
            try {
                const response = await fetch('http://localhost:5000/api/Configuration');
                const data = await response.json();
                setState(data);
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
                console.log(data)
                loadConfigs(data);
            } catch (error) {
                console.error('Error fetching data: ', error);
            }
        };

        fetchConfigs();
    }, []);

    return (
        <ConfigurationContext.Provider value={{ state, setState, configs}}>
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