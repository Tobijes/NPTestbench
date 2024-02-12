import { useState, useEffect, createContext, useContext } from 'react';
import PropTypes from 'prop-types';



const RunProvider = (props) => {

    const callStart = async () => {
        try {
            const response = await fetch('http://localhost:5000/api/Run/start', {
                method: "POST"
            }   );
            const data = await response.json();
            console.log(data)
        } catch (error) {
            console.error('Error fetching data: ', error);
        }
    }

    const callStop = async () => {
        try {
            const response = await fetch('http://localhost:5000/api/Run/stop', {
                method: "POST"
            });
            const data = await response.json();
            console.log(data)
        } catch (error) {
            console.error('Error fetching data: ', error);
        }
    }

    return (
        <RunContext.Provider value={{callStart, callStop}}>
            {props.children}
        </RunContext.Provider>
    );
};


RunProvider.propTypes = {
    children: PropTypes.node.isRequired,
};

const RunContext = createContext();

export const useRunContext = () => useContext(RunContext);

export default RunProvider;