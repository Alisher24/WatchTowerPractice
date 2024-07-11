import React, {useEffect, useRef, useState} from 'react';
import { FFmpeg } from '@ffmpeg/ffmpeg';

interface StreamPlayerProps {
  wsUrl: string;
}

const ffmpeg = new FFmpeg();
// FIXME: error on loading ffmpeg
// await ffmpeg.load();

const StreamPlayer: React.FC<StreamPlayerProps> = ({ wsUrl }) => {
  const [loaded, setLoaded] = useState(false);
  const ffmpegRef = useRef(new FFmpeg());
  const videoRef = useRef<HTMLVideoElement | null>(null);
  const messageRef = useRef<HTMLParagraphElement | null>(null)
    const startStream = async () => {
      const ffmpeg = ffmpegRef.current;
      const ws = new WebSocket(wsUrl);
      ws.binaryType = 'arraybuffer';
      ws.onmessage = async (event) => {
        const videoData = new Uint8Array(event.data);
        console.log("web buffer:", videoData);
        // await ffmpeg.writeFile('input.ts', videoData);
        //
        // await ffmpeg.exec(['-i', 'input.ts', 'output.mp4']);
        //
        // const fileData = await ffmpeg.readFile('output.mp4');
        // const data = new Uint8Array(fileData as ArrayBuffer);
        // if (videoRef.current) {
        //   videoRef.current.src = URL.createObjectURL(
        //     new Blob([data.buffer], { type: 'video/mp4' })
        //   )
        // }
      };

      ws.onclose = () => {
        console.log('WebSocket connection closed');
      };

      ws.onerror = (error) => {
        console.error('WebSocket error:', error);
      };
    };

  return <>
    <video ref={videoRef} className="streamPlayer" controls/>
    <p ref={messageRef}></p>
    <button onClick={startStream}>Load ffmpeg-core</button>
  </>;
};

export default StreamPlayer;