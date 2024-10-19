import React, {useEffect, useState} from 'react'
import {getRealTimeRankings} from './getRealTimeRanking' // 함수 가져오는 용

interface Score {
  username: string
  score: number
}

const Ranking: React.FC = () => {
  const [rankings, setRankings] = useState<Score[]>([])

  useEffect(() => {
    const unsubscribe = getRealTimeRankings(10, (newRankings: Score[]) => {
      setRankings(newRankings)
    })

    return () => {
      unsubscribe()
    }
  }, [])

  return (
    <div>
      <h1>Top Rankings</h1>
      <ul>
        {rankings.map((rank, index) => (
          <li key={index}>
            {index + 1}. {rank.username} - {rank.score}
          </li>
        ))}
      </ul>
    </div>
  )
}

export default Ranking
