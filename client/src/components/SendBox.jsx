import { useState } from 'react';
import { useDataStreamContext } from '../providers/DataStreamProvider';

const SendBox = () => {
  const [message, setMessage] = useState('');
  const dataStreamContext = useDataStreamContext();
  return (
    <div>
      <div>
        <input
          type="text"
          value={message}
          onChange={e => setMessage(e.target.value)}
          placeholder="Message"
        />
        <button onClick={dataStreamContext.sendMessage}>Send</button>
      </div>
      <div className="messages">
        <h2>Messages</h2>
        {dataStreamContext.messages.map((m, index) => (
          <p key={index}>{m.message}</p>
        ))}
      </div>
    </div>
  );
}

export default SendBox;