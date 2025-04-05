import dayjs from "dayjs";

type Props = {
  dateTime?: string;
};

export const DateTimeDisplay = ({ dateTime }: Props) =>
  dateTime != null ? dayjs(dateTime).format(`DD/MM/YYYY HH:mm:ss`) : "-";
