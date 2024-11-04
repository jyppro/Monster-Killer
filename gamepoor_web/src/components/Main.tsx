import React from 'react';
import { Link } from 'react-router-dom';
import '../styles/sign-up.css';
import '../styles/login.css';
import video1 from '../video/bg.mp4';
import Header from '../components/Header';
import Footer from '../components/Footer';
import '../styles/main.css';

const Main: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  return (
    <div className="main-container">
      <Header />
      <div className="bg-video">
        <video className="bg-video__content" autoPlay muted loop>
          <source src={video1} type="video/mp4" />
        </video>
      </div>
      <section className="m">
        <h1 className="fade-in12">Monster Killer</h1>
        <h2 className="fade-in22">Hunt, Competition</h2>
        <p className="fade-in22">"Monster Hunt"</p>
        <p className="fade-in22">New monsters, New skills</p>
      </section>
      <div className="button-container">
        <Link to="/loginform" className="main-button">
          로그인
        </Link>
        <Link to="/signup" className="main-button">
          회원가입
        </Link>
      </div>

      {children}
      <Footer />
    </div>
  );
};

export default Main;
