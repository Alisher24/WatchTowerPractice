import React, {useState} from 'react';
import {PlayIcon, StopIcon} from '../../shared/icons/icons.tsx';
import './Camera.css';
import {StyledInput} from "../../components/StyledInput";

interface CameraProps {
  camera: {
    title: string;
    id: string;
    name: string;
    ip: string;
    password: string;
    isActive: boolean;
  };
  onStartStream: (camera: any) => void;
  onStopStream: (camera: any) => void;
  onEditCamera: (camera: any) => void;
  onDeleteCamera: (camera: any) => void
}

const Camera: React.FC<CameraProps> = ({camera, onStartStream, onStopStream, onEditCamera, onDeleteCamera}) => {

  const [editMode, setEditMode] = useState(false);
  const [editName, setEditName] = useState(camera.name);
  const [editTitle, setEditTitle] = useState(camera.title);
  const [editIP, setEditIP] = useState(camera.ip);
  const [editPassword, setEditPassword] = useState(camera.password);

  const handleEditClick = () => {
    setEditMode(true);
  };

  const handleSaveChanges = () => {
    const updatedCamera = {
      ...camera,
      title: editTitle,
      name: editName,
      ip: editIP,
      password: editPassword,
    };
    onEditCamera(updatedCamera);
    setEditMode(false);
  };

  const handleCancelEdit = () => {
    // Reset edit fields to current camera values
    setEditTitle(camera.title);
    setEditName(camera.name);
    setEditIP(camera.ip);
    setEditPassword(camera.password);
    setEditMode(false);
  };

  const handleDeleteCamera = () => {
    onDeleteCamera(camera);
  };

  return (
    <li className="camera-item">
      <h4>{camera.title}</h4>
      <div>
        {!camera.isActive ?
          <button
            onClick={() => onStartStream(camera)}
            className="start-button" style={{
            background: "green",
            height: "40px",
            width: '40px'
          }}
          >
            <PlayIcon/>
          </button>
          :
          <button onClick={() => onStopStream(camera)} className="stop-button" style={{
            background: "#ff3d3d",
            height: "40px",
            width: '40px'
          }}>
            <StopIcon/>
          </button>
        }
        <button onClick={handleEditClick} className="edit-button">
          Edit
        </button>
      </div>

      {editMode && (
        <div className="edit-popup">
          <div className="edit-popup-content">
            <h2>Edit Camera</h2>
            <div style={{display: 'flex', alignItems: 'center', justifyContent: 'center', flexDirection: "column"}}>
              <StyledInput style={{display: 'block'}}
                           onChange={(e) => setEditTitle(e.target.value)}
                           type="text"
                           value={editTitle}
                           name={'title'}
                           required
                           id={'title'}
                           error={undefined}
                           textLabel={'Title'}/>
              <StyledInput
                onChange={(e) => setEditName(e.target.value)}
                type="text"
                value={editName}
                name={'name'}
                required
                id={'name'}
                error={undefined}
                textLabel={'Name'}/>
              <StyledInput
                onChange={(e) => setEditIP(e.target.value)}
                type="text"
                value={editIP}
                name={'ip'}
                required
                id={'ip'}
                error={undefined}
                textLabel={'IP'}/>
              <StyledInput
                onChange={(e) => setEditPassword(e.target.value)}
                type="password"
                value={editPassword}
                name={'password'}
                required
                id={'password'}
                error={undefined}
                textLabel={'Password'}/>
            </div>
            <div style={{display: "flex", justifyContent: "center"}}>
              <button onClick={handleSaveChanges}>Save Changes</button>
              <button onClick={handleCancelEdit}>Cancel</button>
              <button onClick={handleDeleteCamera} style={{background: "#ff3d3d"}}>Delete</button>
            </div>
          </div>
        </div>
      )}
    </li>
  );
};

export default Camera;
