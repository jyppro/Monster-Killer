import {ref, set} from 'firebase/database'
import {db} from '../FirebaseConfig'

interface ScoreData {
  username: string
  score: number
}

export async function addScore(
  userId: string,
  username: string,
  score: number
): Promise<void> {
  try {
    await set(ref(db, `scores/${userId}`), {
      username,
      score
    } as ScoreData)
    console.log('점수 추가에 성공')
  } catch (error) {
    console.error('점수 추가 실패: ', error)
  }
}
