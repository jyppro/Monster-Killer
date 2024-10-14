// src/components/Screenshot1.tsx
import React from 'react';
import screenshot1 from '../images/screenshot1.png'; // 스크린샷 이미지 경로
import '../styles/screenshot.css';

const Screenshot1: React.FC = () => {
  return (
    <div className="screenshot-container">
      <h1>스크린샷 1</h1>
      <img src={screenshot1} alt="스크린샷 1" className="screenshot-image" />
    </div>
  );
};

export default Screenshot1;
