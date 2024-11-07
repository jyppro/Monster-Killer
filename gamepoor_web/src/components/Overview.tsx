import React from 'react'
import '../styles/overview.css'

// import video1 from '../videos/bg_gameplay.mp4';
// import video1 from '../video/bg.mp4'

const Overview: React.FC = () => {
  return (
    <div className="overview-container">
      <div className="overview-content">
        <h1 className="overview-title">게임 개요</h1>
        <p className="overview-content">
          "몬스터 킬러"는 플레이어가 다양한 몬스터를 사냥하고 경쟁하는 게임입니다.
          전략적인 전투와 다채로운 퀘스트를 통해 플레이어에게 몰입감을 제공합니다.
        </p>
        <h2 className="overview-subtitle">주요 특징</h2>
        <ul className="overview-features">
          <li>다양한 몬스터와의 전투</li>
          <li>점수 경쟁 시스템 </li>
        </ul>
        <p className="overview-footer">
          "몬스터 킬러"는 플레이어가 새로운 경험을 통해 게임의 재미를 극대화할 수 있도록
          설계되었습니다.
        </p>
      </div>
    </div>
  )
}

export default Overview
