import React from 'react';
import CameraList from '../../components/Camera/CameraList';
import CameraRegister from '../../components/Camera/CameraRegister';
import './HomePage.css';
import {Link} from "react-router-dom";
const HomePage: React.FC = () => {
  const isAuthenticated = Boolean(localStorage.getItem('token'));

  return (
    <div className="homePage">
      {isAuthenticated ? (
        <>
          <div className="sidebar">
            <CameraRegister/>
            <CameraList/>
          </div>
          <div className="content">
            <img src="https://github.com/fluidicon.png" alt="example image"/>
          </div>
        </>
      ) : (
        <div style={{ margin: '40vh auto' }}>
          <p >Please login to view and register cameras.</p>
          <Link to="/login">Login</Link>
        </div>
      )}
      
    </div>
  );
};

export default HomePage;