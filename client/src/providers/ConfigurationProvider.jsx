import { useState, useEffect, createContext, useContext } from 'react';
import PropTypes from 'prop-types';
import { apiDelete, apiGet, apiPost, apiPut } from '../libs/api';

const ConfigurationProvider = (props) => {
    const [activeConfiguration, setActiveConfiguration] = useState(null);
    const [configs, setConfigurations] = useState([])
    const [currentConfiguration, setCurrentConfiguration] = useState(null)

    useEffect(() => {
        // This function will update the server with the new active configuration
        const updateServerActiveConfig = async () => {
            if (activeConfiguration) { // Ensure there is an active configuration to update
                await apiPost(`/api/Configuration/active/${activeConfiguration.id}`)
            }
        };

        if (activeConfiguration !== null) { // Check if the state is not null to avoid running on initial load
            updateServerActiveConfig();
        }
    }, [activeConfiguration]); // Run this effect when `state` changes

    const fetchConfigs = async () => {
        const configurations = await apiGet('/api/configuration')
        setConfigurations(configurations);
    };

    useEffect(() => {
        fetchConfigs();
    }, []);


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
        await apiPost(`/api/configuration/${currentConfiguration.id}/parameter/${parameter.id}`, {
            Name: parameter.name,
            Value: parameter.value
        })

    };

    const addParameter = async () => {
        console.log("paramter being added")
        const parameter = await apiPut('/api/Configuration/' + currentConfiguration.id + '/parameter',{
            Name: "",
            Value: "",
        });

        setCurrentConfiguration((prevState) => {
            console.log("prior state.:" + JSON.stringify(prevState))
            return {
                ...prevState,
                parameters: [
                    ...prevState.parameters,
                    parameter
                ],
            };
        });

    };

    const deleteParameter = async (parameterId) => {
        var newParameters = currentConfiguration.parameters.filter((param) => param.id !== parameterId);
        setCurrentConfiguration((prevState) => ({
            ...prevState,
            parameters: newParameters,
        }));
        console.log("this is parm" + parameterId)

        await apiDelete('/api/configuration/' + currentConfiguration.id + '/parameter/' + parameterId)
        console.log("parameter has been beenDeleted " + parameterId)

    };

    useEffect(() => {
        const fetchActiveConfig = async () => {
            const activeConfiguration = await apiGet('/api/configuration/active')

            setActiveConfiguration(activeConfiguration);
            setCurrentConfiguration(activeConfiguration)
        };

        fetchActiveConfig();
    }, []);



    const getConfigById = async (configId) => {
        const configuration = await apiGet('/api/configuration/' + configId)
        setCurrentConfiguration(configuration);
    };


    const cloneConfiguration = async () => {
        const configuration = await apiPost("/api/configuration/" + currentConfiguration.id + "/clone", {})
        setCurrentConfiguration(configuration);
        setConfigurations([...configs, configuration])
    };

    const renameConfiguration = async (name) => {
        const configuration = await apiPost("/api/configuration/" + currentConfiguration.id + "/rename", {
            name: name
        })
        await fetchConfigs();
        setCurrentConfiguration(configuration);
    };

    return (
        <ConfigurationContext.Provider value={
            {
                activeConfiguration,
                setActiveConfiguration,
                configs,
                updateParameter,
                addParameter,
                deleteParameter,
                currentConfiguration,
                setCurrentConfiguration,
                getConfigById,
                cloneConfiguration,
                renameConfiguration
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