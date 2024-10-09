// src/components/Mission.tsx
import React from 'react';
import '../styles/mission.css'; // 스타일 파일 추가

const Mission: React.FC = () => {
  return (
    <div className="mission-container">
      <h1>미션</h1>
      <p>우리의 목표는 모든 플레이어에게 최고의 경험을 제공하는 것입니다.</p>
      <h2>미션 내용</h2>
      <p>몬스터 킬러는 다음과 같은 목표를 가지고 있습니다:</p>
      <ul>
        <li>다양한 몬스터를 탐험하고 사냥할 수 있는 기회 제공</li>
        <li>플레이어 간의 협력을 통해 새로운 경험 창출</li>
        <li>정기적인 업데이트와 이벤트를 통해 지속적인 재미 제공</li>
      </ul>
    </div>
  );
};

export default Mission;
