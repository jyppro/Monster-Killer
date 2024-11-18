// src/components/Groups.tsx
import React from 'react';
import '../styles/groups.css'; // 스타일 파일 추가

const Groups: React.FC = () => {
  return (
    <div className='main-groups'>
    <div className="groups-container">
      <h1>그룹</h1>
      <div className="group">
        <h2>몬스터 헌터 모임</h2>
        <p>설명: 몬스터 헌트를 즐기는 플레이어들이 모여 정보를 공유하고 협력하는 그룹입니다.</p>
        <p>참여 인원: 20명</p>
      </div>
      <div className="group">
        <h2>전략가들</h2>
        <p>설명: 게임 내 전략을 논의하고 발전시키기 위한 그룹입니다. 함께 전략을 세워 승리합시다!</p>
        <p>참여 인원: 15명</p>
      </div>
      <div className="group">
        <h2>커뮤니티 이벤트팀</h2>
        <p>설명: 다양한 게임 관련 이벤트를 계획하고 실행하는 팀입니다. 이벤트에 참여하고 싶다면 연락주세요!</p>
        <p>참여 인원: 10명</p>
      </div>
    </div>
    </div>
  );
};

export default Groups;
