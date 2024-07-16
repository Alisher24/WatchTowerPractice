import React, {useState} from 'react';
import CameraList from '../../components/Camera/CameraList';
import CameraRegister from '../../components/Camera/CameraRegister';
import VideoGrid from '../../components/Camera/VideoGrid';
import StreamPlayer from '../../components/Stream/StreamPlayer';
import apiService from '../../services/api/auth.service';
import './HomePage.css';
import {Link} from "react-router-dom";

interface Camera {
  title: string
  id: number;
  name: string;
  ip: string;
  password: string;
  wsUrl: string;
}

const HomePage: React.FC = () => {
  const [isAuthenticated, setIsAuthenticated] = useState<boolean>(Boolean(localStorage.getItem('token')) || true);
  const [activeStreams, setActiveStreams] = useState<Camera[]>([]);

  const handleStartStream = (camera: Camera) => {
    if (activeStreams.find(stream => stream.id === camera.id)) return; // do nothing if stream is active
    setActiveStreams(prevStreams => [...prevStreams, camera]); // TODO: delete line
    apiService.startStream(camera.ip, camera.name, camera.password)
      .then((url) => {
        console.log("WSURL", url); // debug
        const wsUrl = `ws://localhost:5003/stream/${url}`;
        camera.wsUrl = wsUrl;
        setActiveStreams(prevStreams => [...prevStreams, camera]);
      })
      .catch(error => {
        console.error('Ошибка подключения к камере:', error);
      });
  };
  const handleStopStream = (camera: Camera) => {
    setActiveStreams(prevStreams => prevStreams.filter(stream => stream.id !== camera.id)); // TODO: delete line

  };


  const exitUser = () => {
    localStorage.removeItem('token');
    setIsAuthenticated(false);
    // window.location.reload();
  };
  return (
    <div className="homePage">
      {isAuthenticated ? (
        <>
          <div className="topRight">
            <button onClick={exitUser}>Exit</button>
          </div>
          <div className="sidebar">
            <CameraList onStartStream={handleStartStream} onStopStream={handleStopStream}/>
            <CameraRegister/>
          </div>
          <div className="content">
            <VideoGrid activeStreams={activeStreams} handleStopStream={handleStopStream}/>
            {/*<div className="video-grid">*/}
            {/*{activeStreams.map((camera, index) => (*/}
            {/*  <div key={index} className="streamContainer">*/}
            {/*    <StreamPlayer wsUrl={`ws://localhost:5003/${camera.id}`}/>*/}
            {/*    /!*<StreamPlayer wsUrl={'ws://localhost:5003/stream/192.168.50.165/ws'}/>*!/*/}
            {/*    <button type="button" className="btn-close" onClick={() => handleStopStream(camera)}>*/}
            {/*      /!*<span className="icon-cross"></span>*!/*/}
            {/*      <div className="cross"><span></span></div>*/}
            {/*      /!*<span className="visually-hidden">Close</span>*!/*/}
            {/*    </button>*/}
            {/*    <center>{camera.title}</center>*/}
            {/*  </div>*/}
            {/*))*/}
            {/*}*/}
            {/*</div>*/}
            {activeStreams.length == 0 && <img style={{alignSelf: 'center'}} src="https://github.com/fluidicon.png" alt="example image"/>}
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