import React from 'react'
import '../styles/faqs.css'

const FAQs: React.FC = () => {
  return (
    <div className='main-faqs'>
    <div className="faqs-container">
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
    </div>
  )
}

export default FAQs
