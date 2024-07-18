import React, {useCallback, useEffect, useRef, useState} from 'react';

interface StreamPlayerProps {
    wsUrl: string;
}
const StreamPlayer: React.FC<StreamPlayerProps> = ({ wsUrl }) => {
    const canvasRef = useRef<HTMLCanvasElement | null>(null);

    // @ts-ignore
    const startStream = () => new JSMpeg.Player(wsUrl, {canvas: canvasRef.current, autoplay: true});


    useEffect(() => {
        let player: any;
        
        const script = document.createElement('script');
        script.src = "/js/jsmpeg.min.js";
        script.onload = () => {
            player = startStream();
            return () => player.destroy();
        };
        document.body.appendChild(script);

        return () => {
            console.log("destroy", player, typeof player)
            // player.stop();
            player.destroy();
            document.body.removeChild(script);
        };
    }, [wsUrl]);

    return <canvas ref={canvasRef} className="streamPlayer" />;
};

// const StreamPlayer: React.FC<StreamPlayerProps> = ({wsUrl}) => {
//     const canvasRef = useRef<HTMLCanvasElement | null>(null);
//     // const strea = useRef(Ht)
//    
//    
//
//
//     const startStream = () => {
//         var canvas = document.createElement("canvas");
//         var url = 'ws://192.168.50.101:5003/stream/58?token=eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IkFsaXNoZXIiLCJuYW1laWQiOiIxNiIsInJvbGUiOiJBZG1pbiIsIm5iZiI6MTcyMTI4NzI0NywiZXhwIjoxNzIxMzczNjQ3LCJpYXQiOjE3MjEyODcyNDcsImlzcyI6IlRlc3RJc3N1ZXIiLCJhdWQiOiJUZXN0QXVkaWVuY2UifQ.iMmmJaRmCaFYEljo0mfGqUrBcIzUZKysLpKkcSAOsxo';
//         document.body.appendChild(canvas)
//         return new JSMpeg.Player(url, {canvas: canvas});
//     };
//     useEffect(() => {
//         const script = document.createElement('script');
//         script.src = "/js/jsmpeg.min.js"
//         document.body.appendChild(script);
//         const stream = startStream();
//
//         return () => {
//             console.log(wsUrl, canvasRef.current)
//             stream.destroy();
//             document.removeChild(script);
//         }
//     }, [])
//
//     const setCanvasRef = useCallback((node: HTMLCanvasElement | null) => {
//         canvasRef.current = node;
//         console.log('set canvas', canvasRef.current, wsUrl);
//     }, [wsUrl]);
//    
//     return <>
//         <canvas ref={setCanvasRef} className="streamPlayer" controls/>
//         {/*<button onClick={startStream}>Load ffmpeg-core</button>*/}
//     </>;
// };

export default StreamPlayer;