// src/components/Screenshot1.tsx
import React from 'react';
import screenshot2 from '../images/screenshot2.png'
import '../styles/screenshot.css';

const Screenshot2: React.FC = () => {
  return (
    <div className="screenshot-container">
      <h1>스크린샷 2</h1>
      <img src={screenshot2} alt="스크린샷 1" className="screenshot-image" />
    </div>
  );
};

export default Screenshot2;
