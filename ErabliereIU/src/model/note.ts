export class Note {
  id: any
  idErabliere: any
  title?: string
  text?: string
  fileExtension?: string
  file?: string
  created?: string
  noteDate?: string | null
  reminderDate?: string | null

  // calculated field
  decodedTextFile?: string
}
