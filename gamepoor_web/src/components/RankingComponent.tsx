import React, {useEffect, useState} from 'react'
import {ref, set, push, get, child, onValue} from 'firebase/database'
import {db} from '../FirebaseConfig'
import '../styles/ranking.css'

interface Ranking {
  name: string
  playerID: string
  score: number
}

const RankingComponent: React.FC = () => {
  const [rankings, setRankings] = useState<Ranking[]>([])
  /*   const [name, setName] = useState('')
  const [score, setScore] = useState<number>(0) */

  useEffect(() => {
    const playerRef = ref(db, 'players')

    // 실시간으로 데이터 가져오기
    onValue(playerRef, snapshot => {
      const data = snapshot.val()
      // console.log(data)
      if (data) {
        // const rankingList: Ranking[] = Object.values(data) as Ranking[]
        const rankingList: Ranking[] = Object.entries(data).map(
          ([key, value]: [string, any]) => ({
            name: key,
            playerID: key,
            score: value?.sumScore ?? 0
          })
        )

        rankingList.sort((a: Ranking, b: Ranking) => b.score - a.score)
        setRankings(rankingList)

        // 정렬된 데이터를 Firebase에 다시 저장
        saveSortedRankings(rankingList)
      }
    })

    //페이지 들어갈때만 데이터가져오기
    /*     const fetchRankings = async () => {
      const playerRef = ref(db)
      try {
        // Firebase에서 특정 위치의 데이터를 가져옴
        const snapshot = await get(child(playerRef, 'players'))
        if (snapshot.exists()) {
          const data = snapshot.val()
          console.log(data)

          // 데이터를 변환하고 정렬
          const rankingList: Ranking[] = Object.entries(data).map(
            ([key, value]: [string, any]) => ({
              name: key,
              playerID: key,
              score: value?.sumScore ?? 0
            })
          )

          // 점수 기준 내림차순 정렬
          rankingList.sort((a, b) => b.score - a.score)
          setRankings(rankingList)

          // 정렬된 데이터를 Firebase에 저장
          saveSortedRankings(rankingList)
        } else {
          console.log('No player data found')
        }
      } catch (error) {
        console.error('Error fetching data:', error)
      }
    }
    fetchRankings() */
  }, [])

  // 정렬된 랭킹 데이터를 Firebase에 저장
  const saveSortedRankings = (sortedRankings: Ranking[]) => {
    const sortedRankingRef = ref(db, 'sorted_rankings')
    set(sortedRankingRef, sortedRankings).catch(error => {
      console.error('Error saving sorted data:', error)
    })
  }

  // 새로운 점수 저장
  /* const saveScore = () => {
    if (name && score) {
      const newScoreRef = push(ref(db, 'rankings'))
      set(newScoreRef, {name, score})
        .then(() => {
          setName('')
          setScore(0)
        })
        .catch(error => {
          console.error('Error saving data:', error)
        })
    } else {
      alert('이름과 점수를 입력하세요.')
    }
  } */

  return (
    <div className="container">
      <h1 className="title">Top Rankings</h1>
      <ul className="list">
        {rankings.map((rank, index) => (
          <li key={index}>
            {index + 1}. {rank.name} - {rank.score} points
          </li>
        ))}
      </ul>

      {/* <h3>Submit a Score</h3>
      <input
        type="text"
        placeholder="Name"
        value={name}
        onChange={e => setName(e.target.value)}
      />
      <input
        type="number"
        placeholder="Score"
        value={score}
        onChange={e => setScore(Number(e.target.value))}
      />
      <button onClick={saveScore}>Save Score</button> */}
    </div>
  )
}

export default RankingComponent
