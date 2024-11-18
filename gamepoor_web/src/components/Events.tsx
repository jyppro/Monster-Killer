// src/components/Events.tsx
import React from 'react';
import '../styles/events.css'; // 스타일 파일 추가

const Events: React.FC = () => {
  return (
    <div className='main-event'>
    <div className="events-container">
      <h1>이벤트</h1>
      <div className="event">
        <h2>이벤트 1: 몬스터 헌트 대회</h2>
        <p>날짜: 2024년 10월 15일</p>
        <p>설명: 최고의 헌터를 가리는 대회에 참여하세요! 참가자에게는 특별한 보상이 주어집니다.</p>
      </div>
      <div className="event">
        <h2>이벤트 2: 커뮤니티 오프라인 모임</h2>
        <p>날짜: 2024년 11월 1일</p>
        <p>설명: 게임을 사랑하는 사람들과 만나고, 다양한 활동을 즐겨보세요!</p>
      </div>
      <div className="event">
        <h2>이벤트 3: 특별 보상 주간</h2>
        <p>날짜: 2024년 12월 1일 - 2024년 12월 7일</p>
        <p>설명: 게임 내에서 특별한 보상을 획득할 수 있는 기회를 놓치지 마세요!</p>
      </div>
    </div>
    </div>
  );
};

export default Events;
