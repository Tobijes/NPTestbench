import { useState, useEffect, createContext, useContext } from 'react';
import { HubConnectionBuilder, HubConnectionState } from '@microsoft/signalr';
import PropTypes from 'prop-types';



const DataStreamProvider = (props) => {
  const [connection, setConnection] = useState(null);
  const [deviceStates, setDeviceStates] = useState({})
  const [runId, setRunId] = useState(undefined);

  useEffect(() => {
    const newConnection = new HubConnectionBuilder()
      .withUrl("http://localhost:5000/DataHub")
      .withAutomaticReconnect()
      .build();

    setConnection(newConnection);
  }, []);

  useEffect(() => {

    const setupConnection = async () => {
      if (connection) {
        try {
          await connection.start();
          console.log('SignalR Connected!');
          connection.on("DataState", (dataState) => {
            setRunId(dataState.runId);
            setDeviceStates(dataState.deviceStates);
          });

        } catch (error) {
          console.error('Connection failed: ', error)
        }
      }
    }
    setupConnection();
  }, [connection]);

  return (
    <DataStreamContext.Provider value={{
      deviceStates,
      runId,
    }}>
      {props.children}
    </DataStreamContext.Provider>
  )

}

DataStreamProvider.propTypes = {
  children: PropTypes.node.isRequired,
};

const DataStreamContext = createContext();
export const useDataStreamContext = () => useContext(DataStreamContext);

export default DataStreamProvider;