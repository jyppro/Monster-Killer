import React from "react";
import "../styles/sign-up.css";
import "../styles/login.css";
// import video1 from '../videos/bg_gameplay.mp4';
import video1 from "../video/background.mp4";

const Main: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  return (
    <div className="main-container">
      <div className="bg-video">
        <video className="bg-video__content" autoPlay muted loop>
          <source src={video1} type="video/mp4" />
        </video>
      </div>

      {children}
    </div>
  );
};

export default Main;
