import React, { useState } from 'react';
import StreamPlayer from '../Stream/StreamPlayer';
import './VideoGrid.css';
import { GearIcon } from "../../shared/icons/icons";

interface VideoGridProps {
  activeStreams: { id: string; title: string }[];
  handleStopStream: (camera: { id: string; title: string }) => void;
}

const VideoGrid: React.FC<VideoGridProps> = ({ activeStreams, handleStopStream }) => {
  const [columns, setColumns] = useState(3);
  const [showSettings, setShowSettings] = useState(false);

  const toggleSettings = () => {
    setShowSettings(!showSettings);
    console.log(activeStreams)
  };

  const handleColumnChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    setColumns(Number(event.target.value));
  };

  return (
    <div className="video-grid-container">
      <button id="settingsButton" onClick={toggleSettings}>
        <GearIcon/>
      </button>
      {showSettings && (
        <div id="settingsPopup" className="settings-popup">
          <label htmlFor="columnSlider">Number of columns:</label>
          <input
            type="range"
            id="columnSlider"
            min="1"
            max="5"
            value={columns}
            style={{margin: "0"}}
            onChange={handleColumnChange}
          />
        </div>
      )}
      <div
        className="video-grid"
        style={{ gridTemplateColumns: `repeat(${columns}, 1fr)` }}
      >
        {activeStreams.map((camera, index) => (
          <div key={index} className="streamContainer">
            <StreamPlayer wsUrl={`ws://192.168.50.101:5003/stream/${camera.id}?token=${localStorage.getItem("token")?.slice(1).slice(0, -1)}`} /> {/* FIXME: send token wia ws */}
            <button
              type="button"
              className="btn-close"
              onClick={() => handleStopStream(camera)}
            >
              <div className="cross">
                <span></span>
              </div>
            </button>
            <center>{camera.title}</center>
          </div>
        ))}
      </div>
    </div>
  );
};

export default VideoGrid;
