import React, {useState} from 'react';
import CameraList from '../../components/Camera/CameraList';
import CameraRegister from '../../components/Camera/CameraRegister';
import VideoGrid from '../../components/Camera/VideoGrid';
// import StreamPlayer from '../../components/Stream/StreamPlayer';
import apiService from '../../services/api/auth.service';
import './HomePage.css';
import {Link} from "react-router-dom";

interface Camera {
  title: string
  id: string;
  name: string;
  ip: string;
  password: string;
  wsUrl: string;
  isActive: boolean;
}

const HomePage: React.FC = () => {
  const [isAuthenticated, setIsAuthenticated] = useState<boolean>(Boolean(localStorage.getItem('token')));
  const [activeStreams, setActiveStreams] = useState<Camera[]>([]);

  const handleStartStream = (camera: Camera) => {
    if (activeStreams.find(stream => stream.id === camera.id)) return; // do nothing if stream is active
    camera.isActive = true;
    // camera.wsUrl = `ws://localhost:5003/stream/${camera.id}/${localStorage.getItem('token')}`;
    // setActiveStreams(prevStreams => [...prevStreams, camera]); // TODO: delete line
    // apiService.startStream(camera.ip, camera.name, camera.password)
    setActiveStreams(prevStreams => [...prevStreams, camera]);
    // apiService.startStreamByID(camera.id)
    //   .then((url) => {
    //     camera.wsUrl = `ws://localhost:5003/stream/${url}`;
    //     camera.isActive = true;
    //     console.log("WSURL", camera.wsUrl); // debug
    //     setActiveStreams(prevStreams => [...prevStreams, camera]);
    //   })
    //   .catch(error => {
    //     console.error('Ошибка подключения к камере:', error);
    //   });
  };
  const handleStopStream = (camera: Camera) => {
    
    setActiveStreams(prevStreams => {
      let filter: Camera[] = [];
      for (const prevcamera of prevStreams) {
          if( prevcamera.id !== camera.id ) {
            filter.push(prevcamera);
          } else {
            camera.isActive = false;
            prevcamera.isActive = false;
          }
      }
      // const filter = prevStreams.filter(stream => stream.id !== camera.id).map( stream => {
      //   stream.isActive = false;
      //   return stream;
      // })
      // console.log(prevStreams, camera, filter, 'filter')
      return filter;
    });
  };


  const exitUser = () => {
    localStorage.removeItem('token');
    setIsAuthenticated(false);
  };

  return (
    <div className="homePage">
      {isAuthenticated ? (
        <>
          <div className="topRight">
            <button onClick={exitUser}>Exit</button>
          </div>
          <div className="sidebar">
            <CameraRegister/>
            <CameraList onStartStream={handleStartStream} onStopStream={handleStopStream}/>
          </div>
          <div className="content">
            <VideoGrid activeStreams={activeStreams} handleStopStream={handleStopStream}/>
            {activeStreams.length == 0 &&
                <img style={{alignSelf: 'center'}} src="https://github.com/fluidicon.png" alt="example image"/>}
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