import React, {useEffect, useRef, useState} from 'react';
/*import JSMpeg from 'jsmpeg-player';*/
interface StreamPlayerProps {
  wsUrl: string;
}


const StreamPlayer: React.FC<StreamPlayerProps> = ({ wsUrl }) => {
  const canvasRef = useRef<HTMLCanvasElement | null>(null);

  useEffect(() => {
    const script = document.createElement('script');
    script.src = "/js/jsmpeg.min.js";
    script.onload = () => {
      // @ts-ignore
      const player = new JSMpeg.Player(wsUrl, { canvas: canvasRef.current});

      return () => {
        player.destroy();
      };
    };
    document.body.appendChild(script);

    return () => {
      document.body.removeChild(script);
    };
  }, [wsUrl]);

  return <canvas ref={canvasRef} className="streamPlayer" />;
};
export default StreamPlayer;