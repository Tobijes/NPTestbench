import { useState, useEffect, createContext, useContext } from 'react';
import PropTypes from 'prop-types';

const BASE_URL = 'http://localhost:5000'

const CommandProvider = (props) => {

    const callDeviceCommand = async (url) => {
        try {
            const response = await fetch(BASE_URL + url, { method: "POST" });
            if (response.status != 200) {
                const text = await response.text();
                throw new Error(text);
            }
        } catch (error) {
            console.error('Error fetching data: ', error);
        }
    }

    const callOpen = async (deviceId) => {
        callDeviceCommand('/api/command/open/' + deviceId);
    }

    const callClose = async (deviceId) => {
        callDeviceCommand('/api/command/close/' + deviceId);
    }

    const callPulse = async (deviceId) => {
        callDeviceCommand('/api/command/pulse/' + deviceId);
    }

    return (
        <CommandContext.Provider value={{callOpen, callClose, callPulse}}>
            {props.children}
        </CommandContext.Provider>
    );
};


CommandProvider.propTypes = {
    children: PropTypes.node.isRequired,
};

const CommandContext = createContext();

export const useCommandContext = () => useContext(CommandContext);

export default CommandProvider;