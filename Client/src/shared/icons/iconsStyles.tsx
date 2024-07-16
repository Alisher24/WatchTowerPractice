import styled from "@emotion/styled";


export const IconStyle = styled('svg')`
  margin-right: ${props=>(props.mr || '10')}px;
  margin-left: ${props=>(props.ml || '10')}px;
  margin-top: ${props=>(props.mt || '0')}px;
  margin-bottom: ${props=>(props.mb || '0')}px;
  width: ${props=>(props.wd || '20')}px;
  height: ${props=>(props.hg || '20')}px;
`;

export const TrashIc = styled('svg')`
  width: 50px;
  height: 50px;
   margin-top: 5px;
`;
