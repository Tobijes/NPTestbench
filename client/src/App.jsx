import React, { useState, useEffect } from 'react';
import { HubConnectionBuilder, HubConnectionState } from '@microsoft/signalr';
import './App.css'


function App() {
  const [connection, setConnection] = useState(null);
  const [message, setMessage] = useState('');
  const [messages, setMessages] = useState([]);

  useEffect(() => {
    const newConnection = new HubConnectionBuilder()
    .withUrl("http://localhost:5000/DataHub")      
    .withAutomaticReconnect()
      .build();

    setConnection(newConnection);
  }, []);

  useEffect(() => {
    if (connection) {
      connection.start()
        .then(() => {
          console.log('SignalR Connected!');
          connection.on("ReceiveMessage", (message) => {
            const newMessage = { message };
            setMessages(messages => [...messages, newMessage]);
          });
        })
        .catch(err => console.error('Connection failed: ', err));
    }
  }, [connection]);

  const sendMessage = async () => {
    if (connection.state === HubConnectionState.Connected) {
      try {
        await connection.send("SendMessage", message);
        setMessage('');
      } catch (e) {
        console.error(e);
      }
    } else {
      alert("No connection to server yet.");
    }
  };

  return (
    <div className="App">
      <header className="App-header">
        <h1>SignalR Demo</h1>
        <div>
          <input
            type="text"
            value={message}
            onChange={e => setMessage(e.target.value)}
            placeholder="Message"
          />
          <button onClick={sendMessage}>Send</button>
        </div>
        <div className="messages">
          <h2>Messages</h2>
          {messages.map((m, index) => (
            <p key={index}>{m.message}</p>
          ))}
        </div>
      </header>
    </div>
  );
}

export default App
