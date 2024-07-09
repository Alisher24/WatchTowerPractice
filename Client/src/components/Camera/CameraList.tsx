import React, { useEffect, useState } from 'react';
import apiService from '../../services/api/auth.service';
import "./CameraList.css";

// const fakeCameras = [
//   { id: 1, name: 'Camera 1', ip: '192.168.1.101', password: 'password1' },
//   { id: 2, name: 'Camera 2', ip: '192.168.1.102', password: 'password2' },
//   { id: 3, name: 'Camera 3', ip: '192.168.1.103', password: 'password3' },
//   { id: 4, name: 'Camera 4', ip: '192.168.1.104', password: 'password4' },
//   { id: 5, name: 'Camera 5', ip: '192.168.1.105', password: 'password5' },
//   { id: 6, name: 'Camera 6', ip: '192.168.1.106', password: 'password6' },
//   { id: 6, name: 'Camera 6', ip: '192.168.1.106', password: 'password6' },
//   { id: 6, name: 'Camera 6', ip: '192.168.1.106', password: 'password6' },
//   { id: 6, name: 'Camera 6', ip: '192.168.1.106', password: 'password6' },
//   { id: 6, name: 'Camera 6', ip: '192.168.1.106', password: 'password6' },
//   { id: 6, name: 'Camera 6', ip: '192.168.1.106', password: 'password6' },
//   { id: 6, name: 'Camera 6', ip: '192.168.1.106', password: 'password6' },
//   { id: 6, name: 'Camera 6', ip: '192.168.1.106', password: 'password6' },
//   { id: 6, name: 'Camera 6', ip: '192.168.1.106', password: 'password6' },
//   { id: 6, name: 'Camera 6', ip: '192.168.1.106', password: 'password6' },
// ];
const CameraList: React.FC = () => {
  // const [cameras, setCameras] = useState<any[]>(fakeCameras);
  const [cameras, setCameras] = useState<any[]>([]);
  const [error, setError] = useState<string>('');

  useEffect(() => {
    apiService.getCameras()
      .then(response => {
        setCameras(response.data);
      })
      .catch(err => {
        setError('Failed to fetch cameras');
      });
  }, []);

  const startStream = (cameraId: number) => {
    const camera = cameras.find(c => c.id === cameraId);
    if (camera) {
      apiService.startStream(camera.ip, camera.name, camera.password)
        .then(response => {
          console.log('Stream started:', response.data);
        })
        .catch(err => {
          setError('Failed to start stream');
        });
    } else {
      setError('Camera not found');
    }
  };

  return (
    <div className="cameraList">
      {error && <p style={{ color: 'red' }}>{error}</p>}
      <ul>
        {cameras.map(camera => (
          <li key={camera.id}>
            {camera.name}
            <button onClick={() => startStream(camera.id)}>Start Stream</button>
          </li>
        ))}
      </ul>
    </div>
  );
};

export default CameraList;
