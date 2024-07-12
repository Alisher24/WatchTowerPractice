import React, {useState} from 'react';
import CameraList from '../../components/Camera/CameraList';
import CameraRegister from '../../components/Camera/CameraRegister';
import StreamPlayer from '../../components/Stream/StreamPlayer';
import apiService from '../../services/api/auth.service';
import './HomePage.css';
import {Link} from "react-router-dom";

interface Camera {
  id: number;
  name: string;
  ip: string;
  password: string;
  wsUrl: string;
}

const HomePage: React.FC = () => {
  let isAuthenticated = Boolean(localStorage.getItem('token')) || true;
  const [activeStreams, setActiveStreams] = useState<Camera[]>([]);

  const handleStartStream = (camera: Camera) => {
    apiService.startStream(camera.ip, camera.name, camera.password)
      // .then(response => response.text())
      .then((url) => {
        console.log("WSURL", url);
        const wsUrl = `ws://localhost:5003/${url}`;
        camera.wsUrl = wsUrl;
        setActiveStreams(prevStreams => [...prevStreams, camera]);
      })
      .catch(error => {
        console.error('Ошибка подключения к камере:', error);
      });
  };
  const handleStopStream = (camera: Camera) => {
    setActiveStreams([]); // TODO: delete line
    apiService.stopStream()
      .then(() => {
        setActiveStreams([]); // stop all streams
      })
      .catch(error => {
        console.error('Ошибка остановки потока:', error);
      });
  };


  const exitUser = () => {
    localStorage.removeItem('token');
    isAuthenticated = false;
    window.location.reload();
  };
  return (
    <div className="homePage">
      {isAuthenticated ? (
        <>
          <div className="topRight"><button onClick={exitUser}>Exit</button></div>
          <div className="sidebar">
            <CameraRegister/>
            <CameraList onStartStream={handleStartStream} onStopStream={handleStopStream}/>
          </div>
          <div className="content">
            {activeStreams.map((camera, index) => (
              <div key={index} className="streamContainer">
                {/*<StreamPlayer wsUrl={`wss://localhost:7034/${camera.id}`} />*/}
                <StreamPlayer wsUrl={camera.wsUrl}/>
                <button onClick={() => handleStopStream(camera)}>Stop Stream</button>
              </div>
            ))
            }
            {activeStreams.length > 0 ? <></> : <img src="https://github.com/fluidicon.png" alt="example image"/>}
          </div>
        </>
      ) : (
        <div style={{margin: '40vh auto'}}>
          <p>Please login to view and register cameras.</p>
          <Link to="/login">Login</Link>
        </div>
      )}
    </div>
  );
};

export default HomePage;