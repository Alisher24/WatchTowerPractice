import React, {useEffect, useRef, useState} from 'react';
import jsmpeg from "jsmpeg"
interface StreamPlayerProps {
  wsUrl: string;
}


const StreamPlayer: React.FC<StreamPlayerProps> = ({ wsUrl }) => {
  const canvasRef = useRef<HTMLCanvasElement | null>(null);
  const messageRef = useRef<HTMLParagraphElement | null>(null)
    const startStream = async () => {
      const ws = new WebSocket(wsUrl);
      new jsmpeg(ws, {canvas: canvasRef.current})
      ws.binaryType = 'arraybuffer';
      ws.onmessage = async (event) => {
        const videoData = new Uint8Array(event.data);
        console.log("web buffer:", videoData);
      };

      ws.onclose = () => {
        console.log('WebSocket connection closed');
      };

      ws.onerror = (error) => {
        console.error('WebSocket error:', error);
      };
    };

  return <>
    <canvas ref={canvasRef} className="streamPlayer" controls/>
    <p ref={messageRef}></p>
    <button onClick={startStream}>Load ffmpeg-core</button>
  </>;
};

export default StreamPlayer;