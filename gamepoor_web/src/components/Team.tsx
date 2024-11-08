import React from 'react'
import '../styles/team.css'
import video1 from '../video/bg.mp4'

const Team: React.FC = () => {
  return (
    <div className="team-container">
      <div className="bg-video1">
        <video className="bg-video__content1" autoPlay muted loop>
          <source src={video1} type="video/mp4" />
        </video>
      </div>
      <div className="content">
        <h1>팀 소개</h1>
        <div className="team-member">
          <h2>박재영</h2>
          <p>역할: 게임</p>
          <p>설명: 팀장, UnityWebGL 총괄.</p>
        </div>
        <div className="team-member">
          <h2>이인용</h2>
          <p>역할: DB</p>
          <p>설명: 게임 데이터베이스 저장, 처리</p>
        </div>
        <div className="team-member">
          <h2>이신형</h2>
          <p>역할: 프론트엔드, 백엔드</p>
        </div>
        <div className="team-member">
          <h2>윤영찬</h2>
          <p>역할: 프론트엔드, 백엔드</p>
        </div>
      </div>
    </div>
  )
}

export default Team
