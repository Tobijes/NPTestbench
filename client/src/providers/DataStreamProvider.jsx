import React, { useState, useEffect, createContext, useContext } from 'react';
import { HubConnectionBuilder, HubConnectionState } from '@microsoft/signalr';
import PropTypes from 'prop-types';



const DataStreamProvider = (props) => {
  const [connection, setConnection] = useState(null);
  const [messages, setMessages] = useState([]);

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
          connection.on("ReceiveMessage", (message) => {
            const newMessage = { message };
            setMessages(messages => [...messages, newMessage]);
          });

        } catch (error) {
          console.error('Connection failed: ', error)
        }
      }
    }
    setupConnection();
  }, [connection]);

  const sendMessage = async (message) => {
    if (connection.state === HubConnectionState.Connected) {
      try {
        await connection.send("SendMessage", message);
      } catch (e) {
        console.error(e);
      }
    } else {
      alert("No connection to server yet.");
    }
  };

  return (
    <DataStreamContext.Provider value={{
      messages,
      sendMessage
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