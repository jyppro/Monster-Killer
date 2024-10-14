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
          <Link to="">소개</Link>
          <div className="dropdown">
            <Link to="/overview">개요</Link> 
            <Link to="/team">팀 소개</Link> 
            <Link to="/mission">미션</Link>
          </div>
        </li>
        <li className="dropdown-container">
          <Link to="">스크린샷</Link>
          <div className="dropdown">
            <Link to="/screenshot1">스크린샷 1</Link>
            <Link to="/screenshot2">스크린샷 2</Link>
            <Link to="/screenshot3">스크린샷 3</Link>
          </div>
        </li>
        <li className="dropdown-container">
          <Link to="">커뮤니티</Link>
          <div className="dropdown">
            <Link to="/events">이벤트</Link>
            <Link to="/forums">포럼</Link>
            <Link to="/groups">그룹</Link>
          </div>
        </li>
        <li className="dropdown-container">
          <Link to="">지원</Link>
          <div className="dropdown">
            <Link to="/faqs">자주 묻는 질문</Link>
            <Link to="/contact">연락처</Link>
            <Link to="/feedback">피드백</Link>
          </div>
        </li>
      </ul>
    </nav>
  );
};

export default Header;
