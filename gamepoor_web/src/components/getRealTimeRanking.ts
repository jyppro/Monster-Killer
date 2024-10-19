import {ref, onValue, query, orderByChild, limitToLast} from 'firebase/database'
import {db} from '../FirebaseConfig'

interface Score {
  username: string
  score: number
}

export function getRealTimeRankings(
  limitNumber: number,
  callback: (scores: Score[]) => void
): () => void {
  const scoresRef = query(
    ref(db, 'players/playerID/'),
    orderByChild('score'),
    limitToLast(limitNumber)
  )

  const unsubscribe = onValue(scoresRef, snapshot => {
    const scoresArray: Score[] = []

    snapshot.forEach(childSnapshot => {
      const scoreData = childSnapshot.val()

      if (
        scoreData &&
        typeof scoreData.username === 'string' &&
        typeof scoreData.score === 'number'
      ) {
        scoresArray.push(scoreData as Score)
      }
    })

    // 점수 내림차순 정렬
    const sortedScores = scoresArray.sort((a, b) => b.score - a.score)
    callback(sortedScores)
  })

  return unsubscribe
}
