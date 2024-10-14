import React from 'react';
import '../styles/team.css';

const Team: React.FC = () => {
  return (
    <div className="team-container">
      <h1>팀 소개</h1>
      <div className="team-member">
        <h2>박재영</h2>
        <p>역할: 게임</p>
        <p>설명: 팀장, UnityWebGL 총괄.</p>
      </div>
      <div className="team-member">
        <h2>이인용</h2>
        <p>역할: DB</p>
        <p>설명: 게임 데이터베이스 전체</p>
      </div>
      <div className="team-member">
        <h2>이신형</h2>
        <p>역할: 웹</p>
      </div>
      <div className="team-member">
        <h2>윤영찬</h2>
        <p>역할: 웹</p>
      </div>
    </div>
  );
};

export default Team;
