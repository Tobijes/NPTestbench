import { useState, useEffect, createContext, useContext } from 'react';
import { HubConnectionBuilder, HubConnectionState } from '@microsoft/signalr';
import PropTypes from 'prop-types';

const DataStreamProvider = (props) => {
  const [connection, setConnection] = useState(null);
  const [deviceStates, setDeviceStates] = useState({})
  const [runId, setRunId] = useState(undefined);
  const [connected, setConnected] = useState(false);

  useEffect(() => {
    const connection = new HubConnectionBuilder()
      .withUrl("http://localhost:5000/DataHub")
      .withAutomaticReconnect([0, 1500, 3000, 3000, 3000, 3000])
      .build();

    connection.onreconnecting(() => {
      setConnected(false);
    });

    connection.onreconnected(() => {
      setConnected(true);
    });

    setConnection(connection);
  }, []);

  useEffect(() => {

    const setupConnection = async () => {
      if (connection) {
        try {
          await connection.start();
          console.log('SignalR Connected!');
          setConnected(true);
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
      connected
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