import React, {useEffect, useState} from 'react'
import {getRealTimeRankings} from './getRealTimeRanking'
import '../styles/ranking.css'

interface Score {
  username: string
  score: number
}

const Ranking: React.FC = () => {
  const [rankings, setRankings] = useState<Score[]>([])
  const [error, setError] = useState<string | null>(null)

  useEffect(() => {
    try {
      const unsubscribe = getRealTimeRankings(10, (newRankings: Score[]) => {
        setRankings(newRankings)
      })

      return () => {
        unsubscribe()
      }
    } catch (e) {
      setError('랭킹 데이터를 불러오는 중 오류가 발생했습니다.')
    }
  }, [])

  if (error) {
    return <div className="error">{error}</div>
  }

  return (
    <div className="container">
      <h1 className="title">Top Rankings</h1>
      <ul className="list">
        {rankings.map((rank, index) => (
          <li key={index} className="listItem">
            {index + 1}. {rank.username} - {rank.score}
          </li>
        ))}
      </ul>
    </div>
  )
}

export default Ranking
