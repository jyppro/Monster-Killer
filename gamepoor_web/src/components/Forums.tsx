// src/components/Forums.tsx
import React from 'react';
import '../styles/forums.css'; // 스타일 파일 추가

const Forums: React.FC = () => {
  return (
    <div className="forums-container">
      <h1>포럼</h1>
      <div className="forum-post">
        <h2>질문: 몬스터 헌팅을 위한 최고의 팁!</h2>
        <p>작성자: 사용자123</p>
        <p>날짜: 2024년 10월 01일</p>
        <p>
          몬스터 헌팅에 대해 어떤 팁이 있나요? 저는 처음이라서 어려움을 겪고 있습니다.
          도와주세요!
        </p>
      </div>
      <div className="forum-post">
        <h2>리뷰: 최신 업데이트에 대한 생각</h2>
        <p>작성자: 사용자456</p>
        <p>날짜: 2024년 10월 02일</p>
        <p>
          최신 업데이트에 대한 여러분의 의견은 무엇인가요? 저는 새로운 몬스터와 퀘스트에
          매우 흥미를 느끼고 있습니다.
        </p>
      </div>
      <div className="forum-post">
        <h2>이벤트: 다음 몬스터 헌트 대회 정보</h2>
        <p>작성자: 사용자789</p>
        <p>날짜: 2024년 10월 03일</p>
        <p>
          다음 몬스터 헌트 대회에 대한 정보를 공유해주세요! 참가하고 싶어요.
        </p>
      </div>
    </div>
  );
};

export default Forums;
