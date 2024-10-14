import React from "react";
import "../styles/overview.css";
// import video1 from '../videos/bg_gameplay.mp4';
import video1 from "../video/background.mp4";

const Overview: React.FC = () => {
  return (
    <div className="overview-container">
      {/* 백그라운드 비디오 */}
      <div className="bg-video">
        <video className="bg-video__content" autoPlay muted loop>
          <source src={video1} type="video/mp4" />
        </video>
      </div>

      <div className="overview-content">
        <h1 className="overview-title">게임 개요</h1>
        <p className="overview-content">
          "몬스터 킬러"는 플레이어가 다양한 몬스터를 사냥하고 경쟁하는 액션 RPG
          게임입니다. 이 게임은 전략적인 전투와 다채로운 퀘스트를 통해
          플레이어에게 몰입감을 제공합니다.
        </p>
        <h2 className="overview-subtitle">주요 특징</h2>
        <ul className="overview-features">
          <li>다양한 몬스터와의 전투</li>
          <li>커스터마이즈 가능한 캐릭터</li>
          <li>협력 멀티플레이어 모드</li>
          <li>주기적인 이벤트와 업데이트</li>
        </ul>
        <p className="overview-footer">
          "몬스터 킬러"는 플레이어가 새로운 경험을 통해 게임의 재미를 극대화할
          수 있도록 설계되었습니다.
        </p>
      </div>
    </div>
  );
};

export default Overview;
