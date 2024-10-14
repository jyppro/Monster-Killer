import React, { useEffect } from 'react';

const Game: React.FC = () => {
  useEffect(() => {
    const script = document.createElement('script');
    script.src = '/build/Downloads.loader.js'; // 빌드된 유니티 로더 스크립트 경로
    script.onload = () => {
      // Unity WebGL 라이브러리가 로드된 후에 createUnityInstance 함수 호출
      createUnityInstance(document.querySelector("#unity-canvas"), {
        dataUrl: "/build/Downloads.data",
        frameworkUrl: "/build/Downloads.framework.js",
        codeUrl: "/build/Downloads.wasm",
      }).then((unityInstance) => {
        // Unity instance 생성 성공
        console.log('Unity instance 생성 성공');
      }).catch((message) => {
        console.error(message);
      });
    };
    document.body.appendChild(script);
  }, []);

  return (
    <div id="unity-container" className="unity-desktop">
      <canvas id="unity-canvas" width="960" height="600" style={{ width: '100%', height: '100vh' }} />
    </div>
  );
};

export default Game;
