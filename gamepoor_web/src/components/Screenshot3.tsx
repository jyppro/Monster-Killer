// src/components/Screenshot1.tsx
import React from 'react';
import screenshot3 from '../images/screenshot3.png' // 스크린샷 이미지 경로
import '../styles/screenshot.css';

const Screenshot3: React.FC = () => {
  return (
    <div className="screenshot-container">
      <h1>스크린샷 3</h1>
      <img src={screenshot3} alt="스크린샷 1" className="screenshot-image" />
    </div>
  );
};

export default Screenshot3;
