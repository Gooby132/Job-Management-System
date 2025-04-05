import { Title } from "@mantine/core"

type Props = {
  title: string
}

export const PageTitle = ({
  title
}: Props) => {
  return (
    <Title c={"blue"}>{title}</Title>
  )
}