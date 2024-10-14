// src/components/FAQs.tsx
import React from 'react'
import '../styles/faqs.css' // 스타일 파일 추가
import video1 from '../videos/background.mp4'

const FAQs: React.FC = () => {
  return (
    <div className="faqs-container">
      <div className="bg-video">
        <video className="bg-video__content" autoPlay muted loop>
          <source src={video1} type="video/mp4" />
        </video>
      </div>
      <h1>자주 묻는 질문</h1>
      <h2>Q: 몬스터 킬러는 어떤 게임인가요?</h2>
      <p>
        A: 몬스터 킬러는 헌트모드, 가디언모드, 킬러모드 다양한 모드가 있는 게임입니다.
      </p>

      <h2>Q: 게임은 무료인가요?</h2>
      <p>A: 누구나 쉽게 계정을 만들어 이 게임을 즐길 수 있습니다.</p>

      <h2>Q: 친구와 함께 플레이할 수 있나요?</h2>
      <p>A: 이 게임은 랭킹시스템을 통해 사람들과 점수 경쟁을 하게 만들었습니다.</p>
    </div>
  )
}

export default FAQs
