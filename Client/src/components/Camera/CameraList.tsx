import React, {useEffect, useState} from 'react';
import apiService from '../../services/api/auth.service';
import "./CameraList.css";
import {PlayIcon} from "../../shared/icons/icons.tsx";
import CameraComp from "./Camera.tsx";

const fakeCameras: Camera[] = [
  {title: "Basic camera1", id: '1', name: 'Camera 1', ip: '192.168.1.101', password: 'password1', isActive: false},
  {title: "Basic camera2", id: '2', name: 'Camera 2', ip: '192.168.1.102', password: 'password2', isActive: false},
  {title: "Basic camera3", id: '3', name: 'Camera 3', ip: '192.168.1.103', password: 'password3', isActive: false},
  {title: "Basic camera4", id: '4', name: 'Camera 4', ip: '192.168.1.104', password: 'password4', isActive: false},
  {title: "Basic camera5", id: '5', name: 'Camera 5', ip: '192.168.1.105', password: 'password5', isActive: false},
  {title: "Basic camera6", id: '6', name: 'Camera 6', ip: '192.168.1.106', password: 'password6', isActive: false},
];

interface Camera {
  title: string
  id: string;
  name: string;
  ip: string;
  password: string;
  isActive: boolean
}

interface CameraListProps {
  onStartStream: (camera: Camera) => never;
  onStopStream: (camera: Camera) => never;
}

const CameraList: React.FC<CameraListProps> = ({onStartStream, onStopStream}) => {
  const [cameras, setCameras] = useState<Camera[]>([]);
  const [error, setError] = useState<string>('');
  const getCameras = () => {
    apiService.getCameras()
      .then(response => {
        if (response.status === 204) return setCameras([]);
        const data = response.data.map(camera => {
          camera.title = camera.title;
          return camera;
        })
        setCameras(data);
      })
      .catch((err) => {
        console.log(err)
        setError('Failed to fetch cameras');
      });
  };
  useEffect(() => {
    getCameras()
  }, []);

  const updateCamera = (updatedCamera: Camera) => {
    { // FIXME: update even if no changes or api verification 
      const updatedCameras = cameras.map(camera =>
        camera.id === updatedCamera.id ? updatedCamera : camera
      );
      setCameras(updatedCameras);
    }
    apiService.updateCamera(updatedCamera)
      .then(response => {
        const updatedCameras = cameras.map(camera =>
          camera.id === updatedCamera.id ? updatedCamera : camera
        );
        setCameras(updatedCameras);
        getCameras();
      })
      .catch(err => {
        console.error('Failed to update camera', err);
      });
  };

  const handleEditCamera = (camera: Camera) => {
    updateCamera(camera);
  };

  const handleDeleteCamera = (camera: Camera) => {
    console.log("Deleting camera", camera);
    // { // FIXME: delete without api verification 
    //   const updatedCameras = cameras.filter(other => other.id != camera.id);
    //   console.log(updatedCameras);
    //   setCameras(updatedCameras);
    //   onStopStream(camera);
    // }
    apiService.deleteCamera(camera.id)
      .then(response => {
        const updatedCameras = cameras.filter(prevcamera => camera.id !== prevcamera.id);
        setCameras(updatedCameras);
        onStopStream(camera);
        // getCameras();
      })
      .catch(err => {
        console.error('Failed to delete camera', err);
      });
  };

  return (
    <div className="cameraList">
      {error && <p style={{color: 'red'}}>{error}</p>}
      <ul>
        {(cameras.length == 0 && !error) ?
          <h4>You currently have no cameras available. Please register new camera!</h4>
          :
          cameras.map(camera => (
            <CameraComp key={camera.id} camera={camera}
                        onStartStream={onStartStream}
                        onStopStream={onStopStream}
                        onEditCamera={handleEditCamera}
                        onDeleteCamera={handleDeleteCamera}></CameraComp>
            // <li key={camera.id}>
            //   <h4>{camera.title}</h4>
            //   <div>
            //     <button onClick={() => onStartStream(camera)} style={{
            //       background: "green",
            //       height: "40px",
            //       width: '40px'
            //     }}><PlayIcon/></button>
            //     <button onClick={() => onStopStream(camera)}>Stop</button>
            //     <button onClick={() => handleSettingsClick(camera)}>Settings</button>
            //   </div>
            // </li>

          ))}
      </ul>
    </div>
  );
};

export default CameraList;
