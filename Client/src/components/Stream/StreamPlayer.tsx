// src/components/StreamPlayer.tsx
import React, { useEffect, useRef } from 'react';
import JSMpeg from 'jsmpeg';

interface StreamPlayerProps {
  wsUrl: string;
}

const StreamPlayer: React.FC<StreamPlayerProps> = ({ wsUrl }) => {
  const canvasRef = useRef<HTMLCanvasElement>(null);

  useEffect(() => {
    const player = new JSMpeg.Player(wsUrl, {
      canvas: canvasRef.current
    });

    return () => {
      player.destroy();
    };
  }, [wsUrl]);

  return <canvas ref={canvasRef} />;
};

export default StreamPlayer;