import React, {useEffect, useRef, useState} from 'react';
import JSMpeg from 'jsmpeg-player';
interface StreamPlayerProps {
  wsUrl: string;
}


const StreamPlayer: React.FC<StreamPlayerProps> = ({ wsUrl }) => {
  const canvasRef = useRef<HTMLCanvasElement | null>(null);
    const startStream =  () => {
      new JSMpeg.Player(wsUrl, {canvas: canvasRef.current, autoplay: true})
    };
    startStream();
  return <>
    <canvas ref={canvasRef} className="streamPlayer" controls/>
    {/*<button onClick={startStream}>Load ffmpeg-core</button>*/}
  </>;
};

export default StreamPlayer;