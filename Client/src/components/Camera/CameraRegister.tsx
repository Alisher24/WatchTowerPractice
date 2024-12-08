import React, { useState } from 'react';
import apiService from '../../services/api/auth.service';
import './CameraRegister.css';

const CameraRegister: React.FC = () => {
  const [registerMode, setRegisterMode] = useState(false);
  const [title, setTitle] = useState<string>('');
  const [ip, setIp] = useState<string>('');
  const [name, setName] = useState<string>('');
  const [password, setPassword] = useState<string>('');
  const [error, setError] = useState<string>('');
  const [success, setSuccess] = useState<string>('');
  const handleRegClick = () => {
    setRegisterMode(!registerMode);
  };
  const handleRegister = () => {
    apiService.registerCamera(title, ip, name, password)
      .then(response => {
        setSuccess('Camera registered successfully');
        setError('');
      })
      .catch(err => {
        setError('Failed to register camera');
        setSuccess('');
      });
    
  };

  return (
    <div className="registerButton">
      {registerMode && <div className="cameraRegister">
        <div className="cameraRegister-content">
        {error && <p style={{color: 'red'}}>{error}</p>}
        {success && <p style={{color: 'green'}}>{success}</p>}
        <input
          type="text"
          placeholder="Title"
          value={title}
          onChange={(e) => setTitle(e.target.value)}
        />
        <input
          type="text"
          placeholder="Camera IP"
          value={ip}
          onChange={(e) => setIp(e.target.value)}
        />
        <input
          type="text"
          placeholder="Camera Name"
          value={name}
          onChange={(e) => setName(e.target.value)}
        />
        <input
          type="password"
          placeholder="Password"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
        />
            <div style={{display: "flex", justifyContent: "center", gap: '10px'}}>
        <button onClick={handleRegister}>Register Camera</button>
        <button onClick={handleRegClick}>Cancel</button>
            </div>
        </div>
      </div>}
      <button onClick={handleRegClick}>Register Camera</button>
      
    </div>
  );
};

export default CameraRegister;