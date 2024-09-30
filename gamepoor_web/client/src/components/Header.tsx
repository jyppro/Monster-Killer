import React from 'react';
import { Link } from 'react-router-dom';
import '../styles/navbar.css';

const Header: React.FC = () => {
  return (
    <nav className="navbar">
      <div className="logo">
        <Link to="/">Monster Killer</Link>
      </div>
      <ul className="nav-links">
        <li className="dropdown-container">
          <Link to="/">소개</Link>
          <div className="dropdown">
            <Link to="/intro/overview">개요</Link>
            <Link to="/intro/team">소개</Link>
            <Link to="/intro/mission">미션</Link>
          </div>
        </li>
        <li className="dropdown-container">
          <Link to="/screenshots">스크린샷</Link>
          <div className="dropdown">
            <Link to="/screenshots/1">스크린샷 1</Link>
            <Link to="/screenshots/2">스크린샷 2</Link>
            <Link to="/screenshots/3">스크린샷 3</Link>
          </div>
        </li>
        <li className="dropdown-container">
          <Link to="/community">커뮤니티</Link>
          <div className="dropdown">
            <Link to="/community/events">이벤트</Link>
            <Link to="/community/forums">포럼</Link>
            <Link to="/community/groups">그룹</Link>
          </div>
        </li>
        <li className="dropdown-container">
          <Link to="/support">지원</Link>
          <div className="dropdown">
            <Link to="/support/faqs">자주 묻는 질문</Link>
            <Link to="/support/contact">연락처</Link>
            <Link to="/support/feedback">피드백</Link>
          </div>
        </li>
      </ul>
    </nav>
  );
};

export default Header;
