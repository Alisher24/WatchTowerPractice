import React, { useState } from 'react';
import apiService from '../../services/api/auth.service';
import './CameraRegister.css';

const CameraRegister: React.FC = () => {
  const [ip, setIp] = useState<string>('');
  const [name, setName] = useState<string>('');
  const [password, setPassword] = useState<string>('');
  const [error, setError] = useState<string>('');
  const [success, setSuccess] = useState<string>('');

  const handleRegister = () => {
    apiService.registerCamera(ip, name, password)
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
    <div className="cameraRegister">
      {error && <p style={{ color: 'red' }}>{error}</p>}
      {success && <p style={{ color: 'green' }}>{success}</p>}
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
      <button onClick={handleRegister}>Register Camera</button>
    </div>
  );
};

export default CameraRegister;