import {Rappel} from "./Rappel";

export class Note {
  id: any
  idErabliere: any
  title?: string
  text?: string
  fileExtension?: string
  file?: string
  created?: string
  noteDate?: string | null
  rappel?: Rappel;

  // calculated field
  decodedTextFile?: string
}
